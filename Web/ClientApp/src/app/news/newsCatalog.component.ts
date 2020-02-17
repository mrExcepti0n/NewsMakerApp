import { Component } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NewsRepository } from "../repositories/news.repository";
import { KeyValuePair } from "../models/keyValuePair.model";
import { DictionaryService } from "../services/dictionary.service";
import { Router } from "@angular/router";
import { NewsService } from "../services/news.service";
import { forkJoin } from "rxjs";
import { map } from 'rxjs/operators';
import { Paginator } from "../models/paginator.model";

@Component({
  templateUrl: 'newsCatalog.component.html'
})
export class NewsCatalogComponent {

  public categoryDictionary: KeyValuePair[] = [];

  public selectedCategory: KeyValuePair = new KeyValuePair(0, 'Все');
  public paginator: Paginator = new Paginator();

  public newsCollection: NewsDto[] = [];

  constructor(private newsService: NewsService, dictionaryService: DictionaryService, private router: Router)
  {
    newsService.getNewsCount()
      .subscribe(res => { console.log(res); this.paginator = new Paginator(res) });

    let news = newsService.getNewsCollection();
    let categories = dictionaryService.getCategoryDictionary().pipe(map(res => { res.unshift(this.selectedCategory); this.categoryDictionary = res; return res; }));

    forkJoin([news, categories]).subscribe(results => {
      this.fillCategoryTitle(results[0]);
      this.newsCollection = results[0];
    })
  }

  private fillCategoryTitle(newsCollection: NewsDto[]) {
    newsCollection.forEach(el => el.categoryName = this.categoryDictionary.find(c => c.key == el.categoryId).value);
  }

  loadNewsCollection() {
    this.newsService.getNewsCollection(this.selectedCategory.key === 0 ? null : this.selectedCategory.key)
      .subscribe(res => { this.fillCategoryTitle(res); this.newsCollection = res; });
  }

  deleteNews(id: number) {
    this.newsService.removeNews(id).subscribe();
  }

  changePageSize(size: number) {
    this.paginator.elementPerPage = size;
  }


  getNewsDetail(id: number) {    
    this.router.navigateByUrl(`news/${id}`);
  }
}
