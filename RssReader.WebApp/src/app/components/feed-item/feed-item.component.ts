import { Component, Input } from '@angular/core';
import { FeedItem } from '../../services/models/api/feed-item';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-feed-item',
  standalone: true,
  imports: [DatePipe],
  templateUrl: './feed-item.component.html',
  styleUrl: './feed-item.component.css'
})
export class FeedItemComponent {
  @Input({required: true}) entity!: FeedItem;
  @Input({required: true}) indexNr!: number;
}
