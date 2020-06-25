import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {
  @Input() totalCount: number;
  @Input() pageSize: number; // vijne nga jashte (parent)
  @Output() pageChanged = new EventEmitter<number>(); // output from this component child to parent

  constructor() { }

  ngOnInit() {
  }

  onPagerChanged(event: any) {
    this.pageChanged.emit(event.page);
  }

}
