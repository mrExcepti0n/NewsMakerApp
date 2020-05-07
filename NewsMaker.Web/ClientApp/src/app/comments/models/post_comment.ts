import { v4 as uuid } from 'uuid';

export class PostComment {
  public constructor(content: string, posted: Date, author: string, replies: PostComment[] = []) {
    this.content = content;
    this.posted = posted;
    this.author = author;
    this.replies = replies;
    this.id = uuid();
    this.idChain = [this.id];

    replies.forEach(r => r.idChain.push(this.id));
  }

  public id: string;
  public content: string;
  public posted: Date;
  public author: string;
  public replies: PostComment[];
  public idChain: string[];


  public addReply(comment: PostComment)
  {
    comment.idChain = this.idChain.concat(comment.id);
    this.replies.push(comment);
  }
}
