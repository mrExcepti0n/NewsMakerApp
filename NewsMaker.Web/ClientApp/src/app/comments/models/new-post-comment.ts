import { v4 as uuid } from 'uuid';

export class NewPostComment {
  public constructor(content: string, posted: Date, author: string, parentIds: string[]) {
    this.content = content;
    this.posted = posted;
    this.author = author;
    this.parentIds = parentIds;
    this.id = uuid();
  }

  public id: string;
  public content: string;
  public posted: Date;
  public author: string;
  public parentIds: string[];
}
