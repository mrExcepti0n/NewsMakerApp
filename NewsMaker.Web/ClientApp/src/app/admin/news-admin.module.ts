import { NgModule } from "@angular/core";
import { NewsAdminCatalogComponent } from "./news-admin-catalog.component";
import { RouterModule } from "@angular/router";
import { HttpClientModule } from "@angular/common/http";
import { NewsRepository } from "../repositories/news.repository";
import { NewsService } from "../services/news.service";
import { NewsEditorComponent } from "./news-editor.component";
import { DictionaryService } from "../services/dictionary.service";
import { DictionaryRepository } from "../repositories/dictionary.repository";
import { SharedModule } from "../shared/shared.module";

@NgModule({
  imports: [SharedModule,
    HttpClientModule,
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
