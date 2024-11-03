import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '../models/api/user';
import { Tokens } from '../models/api/tokens';
import { LoggedinUser } from '../models/api/loggedin-user';
import { Folder } from '../models/api/folder';
import { AUTH_REQUIRED } from '../interceptors/auth.interceptor';
import { FeedSubscription } from '../models/api/feed-subscription';
import { FeedItem } from '../models/api/feed-item';
import { Tag } from '../models/api/tag';
import { PaginatedResponse } from '../models/api/paginated-response';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  //private authRequiredContext = new HttpContext().set(AUTH_REQUIRED, true);
  private authNotRequiredContext = new HttpContext().set(AUTH_REQUIRED, false);

  private baseUrl = 'https://localhost:5001';
  private identityUri = 'identity'; private usersUri = 'users';
  private foldersUri = 'folders'; private feedsUri = 'feeds';
  private feedItemsUri = 'feedItems'; private tagsUri = 'tags';
  private feedSubscriptionsUri = 'feedSubscriptions';

  private basePageSize = 10;

  constructor(
    private http: HttpClient) 
  {}

  signup(email: string, password: string, username?: string) {
    const url = `${this.baseUrl}/${this.identityUri}/signup`;
    const credentials = {email, password, username};
    
    return this.http.post<User>(url, credentials, {context: this.authNotRequiredContext});
  }

  verifyEmail(userId: number, otp: string) {
    const url = `${this.baseUrl}/${this.identityUri}/verification`;
    const request = { userId, otp }

    return this.http.post(url, request, {context: this.authNotRequiredContext});
  }

  resendVerificationPassword(userId: number) {
    const url = `${this.baseUrl}/${this.identityUri}/verification?userId=${userId}`;
    
    return this.http.put(url, null, {context: this.authNotRequiredContext});
  }

  login(email: string, password: string) {
    const url = `${this.baseUrl}/${this.identityUri}/login`;
    const request = {email, password};

    return this.http.post<LoggedinUser>(url, request, {context: this.authNotRequiredContext});
  }

  refreshTokens(tokens: Tokens) {
    const url = `${this.baseUrl}/${this.identityUri}/tokens`;
    return this.http.post<Tokens>(url, tokens, {context: this.authNotRequiredContext});
  }

  getUserFolders() {
    const url = `${this.baseUrl}/${this.foldersUri}`;
    return this.http.get<Folder[]>(url);
  }

  getUserTags() {
    const url = `${this.baseUrl}/${this.tagsUri}`;
    return this.http.get<Tag[]>(url);
  }

  createFolder(folderName: string, parentFolderId: number | null) {
    const url = `${this.baseUrl}/${this.foldersUri}`;
    const request = {name: folderName};

    if(parentFolderId != null)
      Object.assign(request, {parentFolderId});

    return this.http.post<Folder>(url, request);
  }

  addFeedSubscription(folderId: number, feedUrl: string, name: string | null) {
    const url = `${this.baseUrl}/${this.foldersUri}/${folderId}/feeds`;
    const request = { feedUrl: feedUrl };

    if(name)
      Object.assign(request, { feedName: name });

    return this.http.post<FeedSubscription>(url, request);
  }

  getFolder(folderId: number) {
    const url = `${this.baseUrl}/${this.foldersUri}/${folderId}`;
    return this.http.get<Folder>(url);
  }

  deleteFolder(folderId: number) {
    const url = `${this.baseUrl}/${this.foldersUri}/${folderId}`;
    return this.http.delete(url);
  }

  getFolderFeedItems(folderId: number, cursor?: string) {
    const url = `${this.baseUrl}/${this.foldersUri}/${folderId}/feedItems`;
    const params = {pageSize: this.basePageSize};

    if(cursor)
      Object.assign(params, {cursor});

    return this.http.get<PaginatedResponse<string, FeedItem[]>>(url, {params: params});
  }

  getUserFeedItems(cursor?: string) {
    const url = `${this.baseUrl}/${this.feedItemsUri}`;
    const params = {pageSize: this.basePageSize};

    if(cursor)
      Object.assign(params, {cursor});

    return this.http.get<PaginatedResponse<string, FeedItem[]>>(url, {params: params});
  }
  
  getTagFeedItems(tagId: number, cursor?: string) {
    const url = `${this.baseUrl}/${this.tagsUri}/${tagId}/feedItems`;
    const params = {pageSize: this.basePageSize};

    if(cursor)
      Object.assign(params, {cursor});

    return this.http.get<PaginatedResponse<string, FeedItem[]>>(url, {params: params});
  }

  getSubscriptionFeedItems(feedId: number, cursor?: string) {
    const url = `${this.baseUrl}/${this.feedSubscriptionsUri}/${feedId}/items`;
    const params = {pageSize: this.basePageSize};

    if(cursor)
      Object.assign(params, {cursor});

    return this.http.get<PaginatedResponse<string, FeedItem[]>>(url, {params: params});
  }

  createTag(name: string) {
    const url = `${this.baseUrl}/${this.tagsUri}`;
    return this.http.post<Tag>(url, {name});
  }

  addTagToFeedSubscription(tagId: number, subscriptionId: number) {
    const url = `${this.baseUrl}/${this.feedSubscriptionsUri}/${subscriptionId}/tags?tagId=${tagId}`;
    return this.http.post(url, null);
  }

  removeTagFromSubscription(tagId: number, subscriptionId: number) {
    const url = `${this.baseUrl}/${this.feedSubscriptionsUri}/${subscriptionId}/tags/${tagId}`;
    return this.http.delete(url);
  }
}
