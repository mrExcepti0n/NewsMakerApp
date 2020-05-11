import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { NewsDto } from "../models/news.model";
import { NewsService } from "../services/news.service";

@Component({
  templateUrl: 'news.component.html'
})
export class NewsComponent implements OnInit {

  public news: NewsDto = new NewsDto();


  constructor(private newsService: NewsService, private route: ActivatedRoute) {
  }

  ngOnInit() {
      this.route.params.subscribe(params => {
        let id = +params.id;
        this.getNews(id);
      });
  }

  getNews(id: number) {
    this.newsService.getNews(id)
                    .subscribe(res => this.news = res);

  }
}
