import { Component, Input, Output, EventEmitter } from "@angular/core";
import { PostComment } from "./models/post-comment";
import { NewPostComment } from "./models/new-post-comment";

@Component({
  selector: 'app-reply-list',
  templateUrl: 'reply-list.component.html',
  styleUrls: ['reply-list.component.css']
})
export class ReplyListComponent {
  togglePanel: any = {};
  @Input()
  public Comments: PostComment[] = [];


  public showTextEditor(commentIndex: number) {
    this.togglePanel[commentIndex] = !this.togglePanel[commentIndex];
  }


  addNewComment(comment: NewPostComment) {

    this.togglePanel = {};
    this.onReplyPosted.emit(comment);
    //this.Comments.push(comment);
  }


  //addNewReply(comment: PostComment) {

  //  this.onReplyPosted.emit(comment);

  //  //this.Comments.push(comment);
  //}


  //@Output()
  //onCommentPosted = new EventEmitter<PostComment>();


  @Output()
  onReplyPosted = new EventEmitter<NewPostComment>();
}
