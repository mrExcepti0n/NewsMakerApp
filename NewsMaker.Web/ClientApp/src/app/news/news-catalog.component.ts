import { Component, OnInit } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NewsService } from "../services/news.service";
import { PagingService } from "../shared/services/paging.service";
import { KeyValue } from "@angular/common";
import { Dictionary } from "../models/dictionary.model";
import { DictionaryRepository } from "../repositories/dictionary.repository";

@Component({
  templateUrl: 'news-catalog.component.html',
  styleUrls: ['news-catalog.component.css'],
  providers: [PagingService]
})
export class NewsCatalogComponent implements OnInit {

  public categoryDictionary: KeyValue<number, string>[] = [];

  public selectedCategory: KeyValue<number, string> = new Dictionary<number, string>(0, 'Все');


  private get selectedCategoryKey(): number {
    return this.selectedCategory.key === 0 ? null : this.selectedCategory.key;;
  }

  public newsCollection: NewsDto[] = [];

  constructor(private newsService: NewsService, private dictionaryRepository: DictionaryRepository, private pagingService: PagingService)
  {
    this.categoryDictionary.push(this.selectedCategory);
  }

  ngOnInit(): void {
    ///todo join methods
    this.getCategories();
    this.getNewsCount();
    this.loadNewsCollection();

    this.pagingService.currentPageChanged$.subscribe(() => this.loadNewsCollection());
    this.pagingService.elementsPerPageChanged$.subscribe(() => this.loadNewsCollection());
  }

  private getNewsCount() {
    this.newsService.getNewsCount(this.selectedCategoryKey)
                    .subscribe(res => { this.pagingService.totalItemsCount = res; });
  }

  private getCategories() {

    if (this.dictionaryRepository.isReady)
      this.categoryDictionary = this.categoryDictionary.concat(this.dictionaryRepository.categoryDictionary);
    else
      this.dictionaryRepository.dictionaryLoaded$.subscribe(x => {
        this.categoryDictionary = this.categoryDictionary.concat(this.dictionaryRepository.categoryDictionary);
      });
  }

  private fillCategoryTitle(newsCollection: NewsDto[]) {
    newsCollection.forEach(el => el.categoryName = this.categoryDictionary.find(c => c.key === el.categoryId).value);
  }

  loadNewsCollection() {
    this.newsService.getNewsCollection(this.selectedCategoryKey, this.pagingService.skip, this.pagingService.take)
                    .subscribe(res => { this.fillCategoryTitle(res); this.newsCollection = res; });
  }


  changeCategory() {
    this.getNewsCount();
    this.loadNewsCollection();
  }
}
