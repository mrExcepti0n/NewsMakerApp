import { Component } from "@angular/core";
import { NewsDto } from "../models/news.model";
import { NgForm } from "@angular/forms";
import { Router, ActivatedRoute } from "@angular/router";
import { NewsRepository } from "../repositories/news.repository";

@Component({
  templateUrl: 'newsEditor.component.html'
})
export class NewsEditorComponent {

  public news: NewsDto = new NewsDto();

  private isEditing: boolean = false;


  public constructor(private newsRepository: NewsRepository, private router: Router, activeRoute: ActivatedRoute) {
    this.isEditing = activeRoute.snapshot.params.mode === 'edit';
    if (this.isEditing) {
      Object.assign(this.news, newsRepository.getNews(activeRoute.snapshot.params.id));
    }
  }


  public saveNews(form: NgForm) {
    if (form.valid) {
      if (this.isEditing) {
        this.newsRepository.updateNews(this.news);
      } else {
        this.newsRepository.addNews(this.news);
      }
    
      this.router.navigateByUrl("");
    }
  }

}
