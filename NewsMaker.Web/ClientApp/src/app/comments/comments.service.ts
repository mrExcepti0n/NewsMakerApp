import { Observable, of } from "rxjs";
import { PostComment } from "./models/post_comment";
import { v4 as uuid } from 'uuid';
import { NewPostComment } from "./models/new-post-comment";

export class CommentsService
{
  private comments: PostComment[] = [];

  constructor() {
    let replyComment = new PostComment('How are you?', new Date('04.05.2020'), 'Dmitry');
   // replyComment.id = uuid();

    let replyComment2 = new PostComment('Hi', new Date('04.05.2020'), 'Maks',  [replyComment]);
    //replyComment2.id = uuid();


    let replyComment3 = new PostComment('Hello', new Date('02.05.2020'), 'Dmitry', [replyComment2]);
    //replyComment3.id = uuid();

    this.comments.push(replyComment3);

    let replyComment4 = new PostComment('By', new Date(Date.now()), 'Sergay');
    //replyComment4.id = uuid();

    this.comments.push(replyComment4);
  }


  postComment(comment: NewPostComment): Observable<NewPostComment> {
    return of(comment);
  }
  public GetComments(postId: number): Observable<PostComment[]> {  

    return of(this.comments);
  }
}
