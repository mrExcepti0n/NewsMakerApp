import { NgModule } from "@angular/core";
import { CommentsComponent } from "./comments.component";
import { CommentsService } from "./services/comments.service";
import { SharedModule } from "../shared/shared.module";
import { ReplyListComponent } from "./reply-list.component";
import { ReplyComponent } from "./reply.component";

@NgModule({
  imports: [SharedModule],
  declarations: [CommentsComponent, ReplyListComponent, ReplyComponent],
  providers: [CommentsService],
  exports: [CommentsComponent]
})
export class CommentsModule {

}
