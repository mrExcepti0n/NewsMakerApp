import { Component } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NgForm } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { NewsRepository } from "../repositories/news.repository";
import { DictionaryService } from "../services/dictionary.service";
import { KeyValuePair } from "../models/keyValuePair.model";

@Component({
  templateUrl: 'newsEditor.component.html'
})
export class NewsEditorComponent {

  public news: NewsDto = new NewsDto();
  public categoryDictionary: KeyValuePair[] = [];

  private isEditing: boolean = false;




  public constructor(private newsRepository: NewsRepository, private dictionaryService: DictionaryService, private router: Router, activeRoute: ActivatedRoute) {

    dictionaryService.getCategoryDictionary().subscribe(res => { console.log(res); this.categoryDictionary = res });

    this.isEditing = activeRoute.snapshot.params.mode === 'edit';
    if (this.isEditing) {
      Object.assign(this.news, newsRepository.getNews(activeRoute.snapshot.params.id));
    }
  }


  compareCategory(item1, item2) {
    return item1 && item2 && item1 == item2;
  }


  public saveNews(form: NgForm) {
    if (form.valid) {
      this.newsRepository.saveNews(this.news);    
      this.router.navigateByUrl("");
    }
  }

}
