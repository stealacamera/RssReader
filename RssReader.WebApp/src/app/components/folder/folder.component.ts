import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Folder } from '../../services/models/api/folder';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../services/api/api.service';
import { ValidationUtils } from '../../services/validation/validation-utils';
import { showErrors } from '../../services/utils/show-errors';
import { Tag } from '../../services/models/api/tag';
import { error } from 'console';

@Component({
  selector: 'app-folder',
  standalone: true,
  imports: [ReactiveFormsModule, FolderComponent],
  templateUrl: './folder.component.html',
  styleUrl: './folder.component.css'
})
export class FolderComponent {
  @Input({required: true}) entity!: Folder;
  @Output() removeEvent: EventEmitter<number> = new EventEmitter();
  @Output() tagCreatedEvent: EventEmitter<Tag> = new EventEmitter();
  @Output() showFolderFeedItemsEvent: EventEmitter<number> = new EventEmitter();
  @Output() showFeedFeedItemsEvent: EventEmitter<number> = new EventEmitter();

  contentsOpened: boolean = false;

  subfolderForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(40)])
  });

  feedForm = new FormGroup({
    url: new FormControl('', [Validators.required, Validators.pattern(ValidationUtils.urlPattern)]),
    name: new FormControl('', [Validators.maxLength(80)])
  });

  tagFormErrors: string[] = [];
  tagForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(30)])
  });

  constructor(
    private api: ApiService,
    private toastr: ToastrService) 
  {}

  showContents(event: Event) {
    const details = document.getElementById((event.target! as Element).id) as HTMLDetailsElement;

    if(!this.contentsOpened && details.open) {
      this.contentsOpened = true;

      this.api.getFolder(this.entity.id)
        .subscribe({
          next: fullEntity => {
              this.entity.feeds = fullEntity.feeds;
              this.entity.subfolders = fullEntity.subfolders;
          },
          error: () => this.toastr.error('Something went wrong, refresh the page')
        })
    }
  }

  toggleSubfolderForm() {
    document.getElementById(`subfolderFormItem-${this.entity.id}`)?.classList.toggle('d-none');
    this.toggleDetails();
  }

  toggleFeedForm() {
    document.getElementById(`feedFormItem-${this.entity.id}`)?.classList.toggle('d-none');
    this.toggleDetails();
  }

  toggleTagsForm = (subscriptionId: number) =>
    document.getElementById(`tags-input-${subscriptionId}`)?.classList.toggle('d-none');

  addTagToFeed(subscriptionId: number) {
    this.tagForm.markAllAsTouched();
    
    if(this.tagForm.invalid) {
      this.toastr.error('Make sure the tag doesn\'t exceed 30 characters');
      return;
    }

    this.api.createTag(this.tagForm.controls.name.value!)
            .subscribe({
              next: newTag => {
                // Send the new tag to parent
                this.tagCreatedEvent.emit(newTag);

                // Add tag to feed
                this.api.addTagToFeedSubscription(newTag.id, subscriptionId)
                        .subscribe({
                          next: () => (this.entity
                                          .feeds
                                          .find(feed => feed.feedSubscriptionId == subscriptionId)!.tags ||= [])
                                          .push(newTag),
                          error: () => this.toastr.error('Make sure the tag isn\'t already attached to the feed')
                        });
              },
              error: err => showErrors(err, this.tagFormErrors, this.toastr)
            });
  }

  removeTagFromFeed(subscriptionId: number, tagId: number, event: Event) {
    this.api.removeTagFromSubscription(tagId, subscriptionId)
            .subscribe({
              next: () => (event.target as HTMLElement).parentElement?.remove(),
              error: () => this.toastr.error('Something went wrong')
            });
  }

  showFeedItemsForFolder = (folderId?: number) =>
    this.showFolderFeedItemsEvent.emit(folderId ?? this.entity.id);

  showFeedItemsForFeed = (feedId: number) =>
    this.showFeedFeedItemsEvent.emit(feedId);

  delete() {
    this.api.deleteFolder(this.entity.id)
      .subscribe({
        next: () => this.removeEvent.emit(this.entity.id),
        error: () => this.toastr.error('Something went wrong, please try again in a moment')
      });
  }

  removeSubfolder(subfolderId: number) {
    this.entity.subfolders = this.entity.subfolders ? 
      this.entity.subfolders.filter(e => e.id != subfolderId) : 
      [];
  }

  addSubfolder() {
    this.subfolderForm.markAllAsTouched();

    if(this.subfolderForm.invalid) {
      this.toastr.error("Make sure the folder name doesn't exceed 40 characters");
      return;
    }

    this.api.createFolder(this.subfolderForm.controls.name.value!, this.entity.id)
      .subscribe({
        next: newSubfolder => {
          (this.entity.subfolders ||= []).push(newSubfolder);
          this.subfolderForm.reset();
        },
        error: () => this.toastr.error('Something went wrong, try again in a moment')
      })
  }

  addFeed() {
    if(this.feedForm.invalid) {console.log(this.feedForm.controls.url.errors)
      this.toastr.error('Make sure your url and name are valid');
      return;
    }

    this.api.addFeedSubscription(this.entity.id, this.feedForm.controls.url.value!, this.feedForm.controls.name?.value)
      .subscribe({
        next: newFeed => {
          (this.entity.feeds ||= []).push(newFeed);
          this.feedForm.reset();
        },
        error: () => this.toastr.error('Something went wrong, try again in a moment')
      });
  }

  private toggleDetails() {
    const details = document.getElementById(`folder-${this.entity.id}`) as HTMLDetailsElement;

    if(details == null)
      return;
    else if(!details.open)
      details.open = true;
  }
}
