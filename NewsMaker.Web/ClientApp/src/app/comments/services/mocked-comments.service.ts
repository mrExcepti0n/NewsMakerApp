//import { Observable, of } from "rxjs";
//import { PostComment } from "../models/post_comment";
//import { NewPostComment } from "../models/new-post-comment";

//export class CommentsService
//{
//  private comments: PostComment[] = [];

//  constructor() {
//    let replyComment = new PostComment('How are you?', new Date('04.05.2020'), 'Dmitry');
//    let replyComment2 = new PostComment('Hi', new Date('04.05.2020'), 'Maks',  [replyComment]);


//    let replyComment3 = new PostComment('Hello', new Date('02.05.2020'), 'Dmitry', [replyComment2]);

//    this.comments.push(replyComment3);

//    let replyComment4 = new PostComment('By', new Date(Date.now()), 'Sergay');
//    this.comments.push(replyComment4);
//  }


//  postComment(comment: NewPostComment): Observable<NewPostComment> {
//    return of(comment);
//  }
//  public GetComments(postId: number): Observable<PostComment[]> {  

//    return of(this.comments);
//  }
//}
