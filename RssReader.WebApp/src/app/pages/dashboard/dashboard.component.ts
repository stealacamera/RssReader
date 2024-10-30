import { Component, HostListener, OnInit } from '@angular/core';
import { Folder } from '../../services/models/api/folder';
import { ApiService } from '../../services/api/api.service';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FolderComponent } from '../../components/folder/folder.component';
import { ToastrService } from 'ngx-toastr';
import { FeedItem } from '../../services/models/api/feed-item';
import { FeedItemComponent } from '../../components/feed-item/feed-item.component';
import { Tag } from '../../services/models/api/tag';
import { PaginatedResponse } from '../../services/models/api/paginated-response';
import { FeedTypes } from '../../services/models/feed-types';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [ReactiveFormsModule, FolderComponent, FeedItemComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  formErrors: string[] = [];

  currentfeedItemsCursor: string = '';
  currentFeedType: FeedTypes = FeedTypes.All; 
  currentFeedSourceId: number = 0;

  tags: Tag[] | null = null;
  folders : Folder[] | null = null;
  feedItems: FeedItem[] | null = null;

  addFolderForm = new FormGroup({
    name: new FormControl('', [Validators.required, Validators.maxLength(40)]),
  });

  constructor(
    private api: ApiService,
    private toastr: ToastrService) 
  {}

  ngOnInit(): void {
    this.api.getUserTags()
            .subscribe({
              next: tags => this.tags = tags,
              error: () => this.tags = null
            });

    this.api.getUserFolders()
            .subscribe({
              next: folders => this.folders = folders,
              error: () => this.folders = null
            });

    this.showFeedItems(this.currentFeedType);
  }

  toggleFolderForm(event: MouseEvent) {
    const input = document.getElementById('folderInput');
    const toggleBtn = document.getElementById('folderInputToggle');
    const actionSymbol = (event.target! as Element).firstElementChild;

    if(input == null || toggleBtn == null)
      return;

    const hiddenClass = 'd-none', roundedClass = 'rounded-end-1';

    if(input.classList.contains(hiddenClass)) {
      actionSymbol?.classList.replace('fa-plus', 'fa-xmark');
      
      input.classList.remove(hiddenClass);
      toggleBtn.classList.remove(roundedClass);
    } else {
      actionSymbol?.classList.replace('fa-xmark', 'fa-plus');
      
      input.classList.add(hiddenClass);
      toggleBtn.classList.add(roundedClass);
    }
  }

  addFolder() {
    this.addFolderForm.markAllAsTouched();

    if(this.addFolderForm.invalid) {
      this.toastr.error("Make sure you've entered a name that's not too long");
      return;
    }

    this.api.createFolder(this.addFolderForm.value.name!, null)
      .subscribe({
        next: newFolder => {
          this.folders?.push(newFolder);
        },
        error: () => this.toastr.error('Something went wrong. Please try again in a moment')
      });
  }

  addNewTag = (tag: Tag) => (this.tags ||= []).push(tag);

  deleteFolder(folderId: number) {
    this.folders = this.folders ? 
      this.folders?.filter(e => e.id !== folderId) : 
      [];
  }

  feedOnScroll(): void {
    const feedView = document.getElementById('feed')!;

    if ((feedView.scrollTop + feedView.offsetHeight) >= feedView.scrollHeight) {
      this.showFeedItems(
        this.currentFeedType, 
        this.currentfeedItemsCursor,
        this.currentFeedType != FeedTypes.All ? this.currentFeedSourceId : undefined);
    }
  }

  showFeedItemsForFeed = (feedId: number) =>
    this.showFeedItems(FeedTypes.Feed, undefined, feedId);

  showFeedItemsForFolder = (folderId: number) => 
    this.showFeedItems(FeedTypes.Folder, undefined, folderId);

  showFeedItemsForUser = () => this.showFeedItems(FeedTypes.All);

  showFeedItemsForTag = (tagId: number) => 
    this.showFeedItems(FeedTypes.Tag, undefined, tagId);

  private showFeedItems(feedType: FeedTypes, cursor?: string, entityId?: number) {
    if(this.currentfeedItemsCursor == null && 
      feedType == this.currentFeedType &&
      entityId == this.currentFeedSourceId)
      return; // Have already reached end of feed

    if(!cursor) {
      this.feedItems = null;
      this.currentFeedType = feedType;
      this.currentFeedSourceId = feedType != FeedTypes.All ? entityId! : 0;
    }

    switch(this.currentFeedType) {
      case FeedTypes.All: {
        this.api.getUserFeedItems(cursor)
                .subscribe({
                  next: items => this.updateFeedItems(items, cursor),
                  error: () => this.toastr.error('Something went wrong')
                });
          
        break;
      }
      case FeedTypes.Folder: {
        this.api.getFolderFeedItems(this.currentFeedSourceId, cursor)
                .subscribe({
                  next: folderFeeds => this.updateFeedItems(folderFeeds, cursor),
                  error: () => this.toastr.error('Something went wrong')
                })

        break;
      }
      case FeedTypes.Feed: {
        this.api.getSubscriptionFeedItems(this.currentFeedSourceId, cursor)
                .subscribe({
                  next: items => this.updateFeedItems(items, cursor),
                  error: () => this.toastr.error('Something went wrong')
                });
        
        break;
      }
      case FeedTypes.Tag: {
        this.api.getTagFeedItems(this.currentFeedSourceId, cursor)
                .subscribe({
                  next: feedItems => this.updateFeedItems(feedItems, cursor),
                  error: () => this.toastr.error('Something went wrong')
                });
        
        break;
      }
    }
  }

  private updateFeedItems(newItems: PaginatedResponse<string, FeedItem[]>, cursor?: string) {
    cursor ? 
      this.feedItems?.push.apply(this.feedItems, newItems.items) :
      this.feedItems = newItems.items;

      this.currentfeedItemsCursor = newItems.nextCursor;
  }
}
