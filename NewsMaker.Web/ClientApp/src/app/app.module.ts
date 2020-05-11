import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NewsComponent } from './news/news.component';
import { NewsCatalogComponent } from './news/news-catalog.component';
import { NewsService } from './services/news.service';
import { DictionaryService } from './services/dictionary.service';
import { CommentsModule } from './comments/comments.module';
import { SharedModule } from './shared/shared.module';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    NewsComponent,
    NewsCatalogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    CommentsModule,
    SharedModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'news', component: NewsCatalogComponent, pathMatch: 'full' },
      { path: 'news/:id', component: NewsComponent, pathMatch: 'full' },
      { path: 'admin/news', loadChildren: './admin/news-admin.module#NewsAdminModule' }
    ])
  ],
  providers: [NewsService,  DictionaryService],
  bootstrap: [AppComponent]
})
export class AppModule { }
