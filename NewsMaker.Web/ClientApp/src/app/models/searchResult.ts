export class SearchResult {
  constructor(public searchGroupResults: SearchGroupResult[]) {
  }
}


export class SearchGroupResult {
  constructor(public searchContentResults: SearchContentResult[], public groupTitle: string) {

  }
}

export class SearchContentResult {
  constructor(public header: string, public content: string) {

  }
}



