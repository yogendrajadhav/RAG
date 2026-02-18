import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbar } from "@angular/material/toolbar";
import { MatIconModule } from '@angular/material/icon';
import { MatTab, MatTabGroup, MatTabContent } from "@angular/material/tabs";
import { FileUpload } from "./components/file-upload/file-upload";
import { QueryInterface } from "./components/query-interface/query-interface";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MatToolbar, MatIconModule, MatTab, MatTabGroup, MatTabContent, FileUpload, QueryInterface],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
  protected readonly title = signal('RAG-frontend');
selectedTabIndex=0;
}
