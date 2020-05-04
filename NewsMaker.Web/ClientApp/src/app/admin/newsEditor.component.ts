import { Component } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NgForm } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { NewsRepository } from "../repositories/news.repository";
import { KeyValuePair } from "../models/keyValuePair.model";
import { DictionaryRepository } from "../repositories/dictionary.repository";

@Component({
  templateUrl: 'newsEditor.component.html'
})
export class NewsEditorComponent {

  public news: NewsDto = new NewsDto();
 

  private isEditing: boolean = false;


  public constructor(private newsRepository: NewsRepository, private dictionaryRepository: DictionaryRepository, private router: Router, activeRoute: ActivatedRoute) {
    this.isEditing = activeRoute.snapshot.params.mode === 'edit';
    if (this.isEditing) {
      Object.assign(this.news, newsRepository.getNews(activeRoute.snapshot.params.id));
    }
  }


  getCategories(): KeyValuePair[] {
    return this.dictionaryRepository.getCategoryDictionary();
  }


  compareCategory(item1, item2) {
    return item1 && item2 && item1 == item2;
  }


  public saveNews(form: NgForm) {
    if (form.valid) {
      this.newsRepository.saveNews(this.news);    
      this.router.navigateByUrl("/admin/news");
    }
  }

}
