import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HorizontalTimelinePageComponent } from './horizontal-timeline-page.component';

describe('HorizontalTimelinePageComponent', () => {
  let component: HorizontalTimelinePageComponent;
  let fixture: ComponentFixture<HorizontalTimelinePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HorizontalTimelinePageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HorizontalTimelinePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
