import { Component, ViewChild, OnInit } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NgForm } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { DictionaryRepository } from "../repositories/dictionary.repository";
import { EditorComponent } from "../shared/components/editor.component";
import { KeyValue } from "@angular/common";
import { NewsService } from "../services/news.service";
import { Observable } from "rxjs";

@Component({
  templateUrl: 'news-editor.component.html',
  styleUrls: ['news-editor.component.css']
})
export class NewsEditorComponent implements OnInit {

  public news: NewsDto;
  public categories: KeyValue<number, string>[];

  @ViewChild(EditorComponent) newsEditor: EditorComponent;

  private isEditing: boolean = false;


  public constructor(private newsService: NewsService, private dictionaryRepository: DictionaryRepository, private router: Router, private activeRoute: ActivatedRoute) {
   
  }

  ngOnInit(): void {

    if (this.dictionaryRepository.isReady)
      this.categories = this.dictionaryRepository.categoryDictionary;
    else
      this.dictionaryRepository.dictionaryLoaded$.subscribe(x => {
        this.categories = this.dictionaryRepository.categoryDictionary;
      });

    this.activeRoute.params.subscribe(params => {
      this.isEditing = params.mode === 'edit';
      if (this.isEditing) {
        const newsId = params.id;
        this.getNews(newsId);
      } else {
        this.news = new NewsDto();
      }
    });
  }

  private getNews(id: number) {
    this.newsService.getNews(id)
      .subscribe(result => {
        this.news = result;
      });
  }


  compareCategory(item1, item2) {
    return item1 && item2 && item1 === item2;
  }


  public saveNews(form: NgForm) {
    if (form.valid) {

      this.news.content = this.newsEditor.content;

      let saveNewsEvent: Observable<any>;
      if (this.news.id == 0) {
        saveNewsEvent = this.newsService.addNews(this.news);
      } else {
        saveNewsEvent = this.newsService.updateNews(this.news);
      }
      saveNewsEvent.subscribe(res => { this.router.navigateByUrl("/admin/news"); });     
    }
  }

}
