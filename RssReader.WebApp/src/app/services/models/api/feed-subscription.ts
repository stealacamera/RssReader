import { Tag } from "./tag";

export interface FeedSubscription {
    feedSubscriptionId: number,
    subscriptionName: string,
    tags: Tag[]
}