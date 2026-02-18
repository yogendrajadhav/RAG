import { Component } from '@angular/core';
import { UploadResponse } from '../../models/document';
import { DocumentService } from '../../services/documentservice';
//import { MaterialModule } from '../../material/material-module';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-file-upload',
  imports: [CommonModule, MatIconModule, MatProgressSpinnerModule, MatCardModule, MatFormFieldModule],
  templateUrl: './file-upload.html',
  styleUrl: './file-upload.scss',
})
export class FileUpload {
  selectedFile:File = null!;
uploading: boolean = false;
uploadResult:UploadResponse | null = null;
errorMessage:string | null = null;

constructor(private documentService: DocumentService) {}

onFileSelected(event:any){
  const file = event.target.files[0];
  if(file && file.type === 'application/pdf'){
    this.selectedFile = file;
    this.errorMessage = null;
  }
  else{
    this.errorMessage = 'Please select a valid PDF file.';
    this.selectedFile = null!;
  }
}

onUpload(){
  if(!this.selectedFile){
    this.errorMessage = 'No file selected. Please choose a PDF file to upload.';
    return;
  }
  if(this.selectedFile){
    this.uploading = true;
    this.documentService.uploadDocument(this.selectedFile).subscribe(
      (response: UploadResponse) => {
        this.uploadResult = response;
        this.uploading = false;
      },
      (error) => {
        this.errorMessage = 'Failed to upload the document. Please try again.';
        this.uploading = false;
      }
    );
  }
}

}
