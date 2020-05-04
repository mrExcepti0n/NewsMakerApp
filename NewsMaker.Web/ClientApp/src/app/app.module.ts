import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NewsComponent } from './news/news.component';
import { NewsCatalogComponent } from './news/newsCatalog.component';
import { NewsService } from './services/news.service';
import { NewsRepository } from './repositories/news.repository';
import { DictionaryService } from './services/dictionary.service';
import { CounterDirective } from './directives/counter.directive';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    NewsComponent,
    NewsCatalogComponent,
    CounterDirective
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'news', component: NewsCatalogComponent, pathMatch: 'full' },
      { path: 'news/:id', component: NewsComponent, pathMatch: 'full' },
      { path: 'admin/news', loadChildren: './admin/newsAdmin.module#NewsAdminModule' }
    ])
  ],
  providers: [NewsService, NewsRepository, DictionaryService],
  bootstrap: [AppComponent]
})
export class AppModule { }
