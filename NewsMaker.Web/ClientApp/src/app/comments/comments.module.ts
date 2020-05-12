import { NgModule } from "@angular/core";
import { CommentsComponent } from "./comments.component";
import { CommentsService } from "./services/comments.service";
import { SharedModule } from "../shared/shared.module";
import { ReplyListComponent } from "./reply-list.component";
import { ReplyComponent } from "./reply.component";
import { SignalrService } from "../services/signalr.service";

@NgModule({
  imports: [SharedModule],
  declarations: [CommentsComponent, ReplyListComponent, ReplyComponent],
  providers: [CommentsService, SignalrService],
  exports: [CommentsComponent]
})
export class CommentsModule {
}
