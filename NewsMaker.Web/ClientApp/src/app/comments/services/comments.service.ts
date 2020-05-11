import { HttpClient, HttpParams } from "@angular/common/http";
import { PostComment } from "../models/post-comment";
import { Observable } from "rxjs";
import { NewPostComment } from "../models/new-post-comment";
import { Injectable } from "@angular/core";


@Injectable()
export class CommentsService {
  private baseUrl = 'http://localhost:3131/comment';

  constructor(private http: HttpClient) {

  }

  getComments(postId: number): Observable<PostComment[]> {
    let parameters = new HttpParams().set('postId', postId.toString());
    return this.http.get<PostComment[]>(this.baseUrl, { params: parameters });
  }


  postComment(comment: NewPostComment): Observable<PostComment> {
    return this.http.post<PostComment>(this.baseUrl, comment);
  }
}
