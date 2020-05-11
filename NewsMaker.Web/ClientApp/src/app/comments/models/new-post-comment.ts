import { Author } from "./author.model";

export class NewPostComment {
  constructor(public postId: number, public content: string, public postedDate: Date, public author: Author, public parentId: string) {

  }
}
