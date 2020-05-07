import { Component, OnInit, Input } from "@angular/core";
import { CommentsService } from "./comments.service";
import { PostComment } from "./models/post_comment";
import { NewPostComment } from "./models/new-post-comment";

@Component({
  selector: 'app-comments',
  templateUrl: 'comments.component.html'
})
export class CommentsComponent implements OnInit {


  @Input()
  private postId: number;
  public Comments: PostComment[] = [];

  public constructor(private comentsService: CommentsService) {

  }
  ngOnInit(): void {
    this.comentsService.GetComments(this.postId)
      .subscribe(res => this.Comments = res);
  }


  addNewComment(comment: NewPostComment) {
    this.addComment(comment);
    //this.Comments.push(comment);  
  }


  addNewReply(newComment: NewPostComment) {
    this.addComment(newComment);
  }


  private addComment(newComment: NewPostComment) {
    let comment = new PostComment(newComment.content, newComment.posted, newComment.author);
    if (newComment.parentIds.length === 0) {
      this.Comments.push(comment);
    }
    else {
      let parentElementId = newComment.parentIds.shift();
      let parentElement = this.Comments.find(c => c.id === parentElementId);

      while (newComment.parentIds.length > 0) {
        let id = newComment.parentIds.shift();
        parentElement = parentElement.replies.find(c => c.id === id);
      }
      comment.idChain = parentElement.idChain.concat(comment.id);
      parentElement.replies.push(comment);
      //parentElement.addReply(comment);
    }
  }
 
}
