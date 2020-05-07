import { Component } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NewsRepository } from "../repositories/news.repository";
import { KeyValuePair } from "../models/keyValuePair.model";
import { DictionaryService } from "../services/dictionary.service";


@Component({
  templateUrl: 'news-admin-catalog.component.html'  ,
  styleUrls: ['../news/news-catalog.component.css', 'news-admin-catalog.component.css']
})
export class NewsAdminCatalogComponent {  

  public categoryDictionary: KeyValuePair[];

  public selectedCategory: KeyValuePair = new KeyValuePair(0, 'Все');

  constructor(private newsRepository: NewsRepository, private dictionaryService: DictionaryService) {

    dictionaryService.getCategoryDictionary().subscribe(res => { res.unshift(this.selectedCategory); console.log(this.selectedCategory); this.categoryDictionary = res; console.log(this.categoryDictionary ) });
    //this.newsCollection = newsRepository.getNewsCollection();
  }


  getNewsCollection(): NewsDto[] {
    let newsCollection = this.newsRepository.getNewsCollection(this.selectedCategory.key === 0 ? null : this.selectedCategory.key);
    newsCollection.forEach(el => el.categoryName = this.categoryDictionary.find(c => c.key == el.categoryId).value);
    return newsCollection;
  }


  deleteNews(id: number) {
    this.newsRepository.removeNews(id);
  }
}
