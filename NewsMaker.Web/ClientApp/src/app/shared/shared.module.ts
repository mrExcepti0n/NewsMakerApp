import { NgModule } from "@angular/core";
import { DateAgoPipe } from "./pipes/date-ago.pipe";
import { CommonModule } from "@angular/common";
import { EditorComponent } from "./components/editor.component";
import { CKEditorModule } from "@ckeditor/ckeditor5-angular";
import { FormsModule } from "@angular/forms";
import { SimpleEditorComponent } from "./components/simple-editor.component";

@NgModule({
  imports: [CommonModule, CKEditorModule, FormsModule],
  declarations: [
    DateAgoPipe,
    EditorComponent,
    SimpleEditorComponent
  ],
  exports: [
    DateAgoPipe,
    SimpleEditorComponent,
    EditorComponent,
    CommonModule,
    FormsModule
  ]
})
export class SharedModule {
  
}
