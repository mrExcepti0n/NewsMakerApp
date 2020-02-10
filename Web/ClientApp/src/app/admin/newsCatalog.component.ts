import { Component } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NewsRepository } from "../repositories/news.repository";

@Component({
  templateUrl: 'newsCatalog.component.html'
})
export class NewsCatalogComponent {
  public newsCollection: NewsDto[];

  constructor(private newsRepository: NewsRepository) {
    this.newsCollection = newsRepository.getNewsCollection();
  }
}
