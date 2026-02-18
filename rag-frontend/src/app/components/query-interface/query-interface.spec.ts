import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QueryInterface } from './query-interface';

describe('QueryInterface', () => {
  let component: QueryInterface;
  let fixture: ComponentFixture<QueryInterface>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [QueryInterface]
    })
    .compileComponents();

    fixture = TestBed.createComponent(QueryInterface);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
