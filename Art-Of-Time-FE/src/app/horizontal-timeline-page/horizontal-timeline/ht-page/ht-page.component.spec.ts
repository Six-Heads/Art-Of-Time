import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HtPageComponent } from './ht-page.component';

describe('HtPageComponent', () => {
  let component: HtPageComponent;
  let fixture: ComponentFixture<HtPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HtPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HtPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
