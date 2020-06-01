import { Component } from "@angular/core";
import { NewsCatalogComponent } from "../news/news-catalog.component";
import { NewsService } from "../services/news.service";
import { PagingService } from "../shared/services/paging.service";
import { DictionaryRepository } from "../repositories/dictionary.repository";


@Component({
  templateUrl: 'news-admin-catalog.component.html'  ,
  styleUrls: ['../news/news-catalog.component.css', 'news-admin-catalog.component.css'],
  providers: [PagingService]
})
export class NewsAdminCatalogComponent extends NewsCatalogComponent{  

  constructor(newsService: NewsService, dictionaryRepository: DictionaryRepository, pagingService: PagingService) {
    super(newsService, dictionaryRepository, pagingService);
  }

  public deleteNews(id: number) {
    this.newsService.removeNews(id).subscribe(res => {
      this.loadNewsCollection();
      this.pagingService.totalItemsCount--;
    });
  }
}
