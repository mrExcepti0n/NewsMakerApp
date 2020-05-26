import { Component, Input, ViewChild, Output, EventEmitter } from "@angular/core";
import { CommentsService } from "./services/comments.service";
import { NewPostComment } from "./models/new-post-comment";
import { SimpleEditorComponent } from "../shared/components/simple-editor.component";
import { PostComment } from "./models/post-comment";
import { Author } from "./models/author.model";
import { v4 as uuid } from 'uuid';

@Component({
  selector: 'app-reply-form',
  templateUrl: 'reply.component.html',
  styleUrls: ['reply.component.css']
})
export class ReplyComponent {

  @ViewChild(SimpleEditorComponent, { static: false }) commentEditor: SimpleEditorComponent;


  @Input('parentId')
  parentId: string = null;

  @Input('postId')
  postId: number;

  authorName: '';

  constructor(private commentService: CommentsService) {

  }

  postComment() {

    let author: Author = { id: uuid(), name: this.authorName };

    const comment = new NewPostComment(this.postId, this.commentContent, new Date(Date.now()), author, this.parentId);

    this.clearInputs();
    this.commentService.postComment(comment)
      .subscribe(res => {
        this.onPosted.emit(res);
      });
  }


  @Output() onPosted = new EventEmitter<PostComment>();

  private get commentContent(): string {
    return this.commentEditor.content;
  }

  private clearInputs() {
    this.authorName = '';
    this.commentEditor.content = '';
  }

}
