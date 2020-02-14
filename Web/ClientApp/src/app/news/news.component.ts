import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NewsRepository } from "../repositories/news.repository";
import { NewsDto } from "../models/news.model";

@Component({
  templateUrl: 'news.component.html'
})
export class NewsComponent {
  private newsId: number;
  private news: NewsDto = new NewsDto();


  constructor(activeRoute: ActivatedRoute, newsRepository: NewsRepository) {
    this.newsId = activeRoute.snapshot.params.id
    this.news = newsRepository.
  }

   

}
