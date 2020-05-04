import { NgModule } from "@angular/core";
import { NewsAdminCatalogComponent } from "./newsAdminCatalog.component";
import { RouterModule } from "@angular/router";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";
import { NewsRepository } from "../repositories/news.repository";
import { NewsService } from "../services/news.service";
import { NewsEditorComponent } from "./newsEditor.component";
import { DictionaryService } from "../services/dictionary.service";
import { DictionaryRepository } from "../repositories/dictionary.repository";

@NgModule({
  imports: [CommonModule,
    HttpClientModule, FormsModule,
    RouterModule.forChild([
      { path: '', component: NewsAdminCatalogComponent },
      { path: ':mode', component: NewsEditorComponent },
      { path: ':mode/:id', component: NewsEditorComponent }
    ])],

  providers: [NewsRepository, NewsService, DictionaryService, DictionaryRepository],
  declarations: [NewsAdminCatalogComponent, NewsEditorComponent]
})
export class NewsAdminModule {

}
