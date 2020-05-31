import { DictionaryService } from "../services/dictionary.service";
import { Injectable } from "@angular/core";
import { Subject } from "rxjs";
import { DictionaryItem } from "../models/dictionary_item.model";


@Injectable({
  providedIn: 'root',
}) 
export class DictionaryRepository {

  public categoryDictionary: DictionaryItem[] = [];

  private dictionaryLoadedSource = new Subject();
  dictionaryLoaded$ = this.dictionaryLoadedSource.asObservable();
  isReady: boolean = false;


  constructor(private dictionaryService: DictionaryService) {
    this.load();
  }


  load() {
    this.dictionaryService.getCategoryDictionary()
      .subscribe(res => {
        this.categoryDictionary = res;
        this.isReady = true;
        this.dictionaryLoadedSource.next();
      });
  }

}
