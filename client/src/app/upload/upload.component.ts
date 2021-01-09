import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { TradeLog } from '../_models/trade-log';
import { MembersService } from '../_services/members.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  displayedColumns: string[] = ['key', 'value'];
  title = 'tradeanalyzer';
  dataSource: any;
  feedback: string;
  uploadForm: FormGroup;
  results: TradeLog[] = [];
  searchText: string;
  constructor(private formBuilder: FormBuilder,
    private memberService: MembersService) {
  }

  ngOnInit(): void {
    this.uploadForm = this.formBuilder.group({
      profile: ['']
    });
  }

  onFileSelect(event): void {
    if (event.target.files.length > 0) {
      const file = event.target.files[0];
      this.uploadForm.get('profile').setValue(file);
    }
  }

  onSubmit(): void {
    const formData = new FormData();
    this.results = [];
    this.feedback = '';
    formData.append('file', this.uploadForm.get('profile').value);
    this.sendTradeLogFile(formData);
  }
  public orderByKey(a) {
    return a.key;
  }
  private sendTradeLogFile(formData: FormData): void {
    this.memberService.sendTradeLogFile(formData).subscribe(
      result => {
        this.results = result;
        this.dataSource = result;
      },
      error => {
        this.feedback = 'This file can not be imported.';
      },
      () => {
        this.feedback = 'Trades have been imported.';
      }
    );
  }
}
