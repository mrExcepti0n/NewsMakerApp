import { Component, Input, ViewChild, Output, EventEmitter } from "@angular/core";
import { CommentsService } from "./comments.service";
import { NewPostComment } from "./models/new-post-comment";
import { SimpleEditorComponent } from "../shared/components/simple-editor.component";


@Component({
  selector: 'app-reply-form',
  templateUrl: 'reply.component.html',
  styleUrls: ['reply.component.css']
})
export class ReplyComponent {

  @ViewChild(SimpleEditorComponent) commentEditor: SimpleEditorComponent;


  @Input('parentIdChain')
  parentIdChain: string[] = [];

  authorName: '';

  constructor(private commentService: CommentsService) {

  }

  postComment() {
    const comment = new NewPostComment(this.commentContent, new Date(Date.now()), this.authorName, this.parentIdChain.slice());

    this.clearInputs();
    this.commentService.postComment(comment)
      .subscribe(res => {
        this.onPosted.emit(res);
      });
  }


  @Output() onPosted = new EventEmitter<NewPostComment>();

  private get commentContent(): string {
    return this.commentEditor.content;
  }

  private clearInputs() {
    this.authorName = '';
    this.commentEditor.content = '';
  }

}
