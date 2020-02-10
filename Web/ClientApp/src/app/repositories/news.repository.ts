import { NewsService } from "../services/news.service";
import { Injectable } from "@angular/core";
import { NewsDto } from "../models/news.model";

@Injectable()
export class NewsRepository {

  private newsCollection: NewsDto[] = [];

  constructor(private newsService: NewsService) {
    newsService.getNewsCollection().subscribe(res => this.newsCollection = res);
  }


  public getNewsCollection(): NewsDto[] {
    return this.newsCollection
  }

  public getNews(id: number): NewsDto {
    var news = this.newsCollection.find(n => n.id == id);
    console.log(news);
    return news;
  }

  public addNews(news: NewsDto) {
    this.newsService.addNews(news);
  }


  public updateNews(news: NewsDto) {
    this.newsService.updateNews(news).subscribe(res => console.log(res), error => console.log(error));
  }
}
