import { NewsService } from "../services/news.service";
import { Injectable } from "@angular/core";
import { NewsDto } from "../models/news.model";

@Injectable()
export class NewsRepository {

  private newsCollection: NewsDto[] = [];

  constructor(private newsService: NewsService) {
    newsService.getNewsCollection().subscribe(res => this.newsCollection = res);
  }


  public getNewsCollection(categoryId?: number): NewsDto[] {
    return this.newsCollection.filter(n => categoryId === null || categoryId === n.categoryId);
  }

  public getNews(id: number): NewsDto {
    var news = this.newsCollection.find(n => n.id == id);
    console.log(news);
    return news;
  }

  public removeNews(id: number) {
    this.newsService.removeNews(id).subscribe(res => this.newsCollection.splice(this.newsCollection.findIndex(n => n.id == id), 1));
  }


  public saveNews(news: NewsDto) {
    if (news.id == 0) {
      this.newsService.addNews(news).subscribe(res => { news.id = res; this.newsCollection.push(news); });    
    } else {
      this.newsService.updateNews(news).subscribe(res => this.newsCollection.splice(this.newsCollection.findIndex(n => n.id == news.id), 1, news));
    }
  }
}
