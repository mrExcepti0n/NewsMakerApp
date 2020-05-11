import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";
import { KeyValue } from "@angular/common";


@Injectable()
export class DictionaryService {
  private url = "/api/Dictionary"

  constructor(private http: HttpClient) {
  }

  getCategoryDictionary(): Observable<KeyValue<number, string>[]> {
    let parameters = new HttpParams().set('refType','1');
    return this.http.get<KeyValue<number, string>[]>(this.url, { params: parameters });
  }
}
