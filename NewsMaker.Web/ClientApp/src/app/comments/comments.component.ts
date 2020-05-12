import { Component, OnInit, Input, OnDestroy } from "@angular/core";
import { CommentsService } from "./services/comments.service";
import { PostComment } from "./models/post-comment";
import { SignalrService } from "../services/signalr.service";
import { Subscription } from "rxjs";

@Component({
  selector: 'app-comments',
  templateUrl: 'comments.component.html'
})
export class CommentsComponent implements OnInit, OnDestroy {


  @Input()
  public postId: number;
  public Comments: PostComment[] = [];

  public constructor(private comentsService: CommentsService, private signalrService: SignalrService) {

  }
  private newPostSubscription: Subscription;

  ngOnInit(): void {
    this.comentsService.getComments(this.postId)
      .subscribe(res => this.Comments = res);

    this.newPostSubscription = this.signalrService.joinGroup(this.postId)
      .subscribe(pc => this.addNewComment(pc));
  }


  ngOnDestroy(): void {
    this.signalrService.leaveGroup(this.postId);
    this.newPostSubscription.unsubscribe();
  }



  addNewComment(comment: PostComment) {
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
