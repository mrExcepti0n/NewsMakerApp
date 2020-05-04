import { Component } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NewsRepository } from "../repositories/news.repository";
import { NewsDto } from "../models/news.model";
import { NewsService } from "../services/news.service";

@Component({
  templateUrl: 'news.component.html'
})
export class NewsComponent {
  private newsId: number;
  private news: NewsDto = new NewsDto();


  constructor(activeRoute: ActivatedRoute, newsService: NewsService) {
    this.newsId = activeRoute.snapshot.params.id;

    newsService.getNews(this.newsId).subscribe(res => this.news = res);
  }  

}
