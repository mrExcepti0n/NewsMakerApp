import { Component, OnInit, Input } from "@angular/core";
import { CommentsService } from "./services/comments.service";
import { PostComment } from "./models/post-comment";

@Component({
  selector: 'app-comments',
  templateUrl: 'comments.component.html'
})
export class CommentsComponent implements OnInit {


  @Input()
  public postId: number;
  public Comments: PostComment[] = [];

  public constructor(private comentsService: CommentsService) {

  }
  ngOnInit(): void {
    this.comentsService.getComments(this.postId)
      .subscribe(res => this.Comments = res);
  }


  addNewComment(comment: PostComment) {
    this.addComment(comment);  
  }


  addNewReply(newComment: PostComment) {
    this.addComment(newComment);
  }


  private addComment(comment: PostComment) {
    if (comment.parents.length === 0) {
      this.Comments.push(comment);
    }
    else {

      let parentElement = this.Comments.find(c => c.id === comment.parents[0]);

      for (let i = 1; i < comment.parents.length; i++) {
        parentElement = parentElement.replies.find(c => c.id === comment.parents[i]);
      }

      parentElement.replies.push(comment);
    }
  }
 
}
