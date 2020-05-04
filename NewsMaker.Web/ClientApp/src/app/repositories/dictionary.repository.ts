import { KeyValuePair } from "../models/keyValuePair.model";
import { DictionaryService } from "../services/dictionary.service";
import { Injectable } from "@angular/core";


@Injectable()
export class DictionaryRepository {

  private categoryDictionary: KeyValuePair[] = [];

  constructor(private dictionaryService: DictionaryService) {
    dictionaryService.getCategoryDictionary().subscribe(res => this.categoryDictionary = res);
  }


  getCategoryDictionary() {
    return this.categoryDictionary;
  }
}
