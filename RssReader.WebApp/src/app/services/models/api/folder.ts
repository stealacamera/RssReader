import { FeedSubscription } from "./feed-subscription";

export interface Folder {
    id: number;
    name: string;
    feeds: FeedSubscription[],
    subfolders: Folder[]
}