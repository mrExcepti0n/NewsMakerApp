import '@ckeditor/ckeditor5-build-classic/build/translations/ru';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { Component, Input, ViewChild } from '@angular/core';
import { CKEditorComponent } from '@ckeditor/ckeditor5-angular';

@Component({
  selector: 'app-simple-editor',
  styleUrls: ['editor.component.css', 'simple-editor.component.css'],
  template: `<ckeditor  #ckEditor
                [editor]='editor'
                [data]='inputContent'
                [config]='config'>
             </ckeditor>`,
})
export class SimpleEditorComponent {
  @ViewChild('ckEditor',  { static: false }) ckEditor: CKEditorComponent;

  @Input()
  inputContent = '';

  private editor = ClassicEditor;

  config: any = {
    toolbar: [ 'bold', 'italic'],
    language: 'ru',
    placeholder: 'Начните писать здесь!'
  };


  public get content() {
    return this.ckEditor.editorInstance.getData();
  }

  public set content(value: string) {
    this.ckEditor.editorInstance.setData(value);
  }

}
