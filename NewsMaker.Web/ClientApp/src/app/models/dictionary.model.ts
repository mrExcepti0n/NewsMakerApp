import { KeyValue } from "@angular/common";

export class Dictionary<tk, tv> implements KeyValue<tk, tv> {
  constructor(public key: tk, public value: tv) {
  
  }
}
