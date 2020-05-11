import { Author } from "./author.model";

export class PostComment {

  public id: string;
  public content: string;
  public postedDate: Date;

  public postId: number;
  public author: Author;
  public replies: PostComment[];
  public parents: string[];
}
