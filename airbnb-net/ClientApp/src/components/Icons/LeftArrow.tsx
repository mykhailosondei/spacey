import * as React from "react";
import type { SVGProps } from "react";
const SvgCalendarLeftArrow = (props: SVGProps<SVGSVGElement>) => (
  <svg
    aria-hidden="true"
    style={{
      height: 12,
      width: 12,
      display: "block",
      fill: "currentcolor",
    }}
    viewBox="0 0 18 18"
    width="1em"
    height="1em"
    {...props}
  >
    <path
      fillRule="evenodd"
      d="M13.7 16.29a1 1 0 1 1-1.42 1.41l-8-8a1 1 0 0 1 0-1.41l8-8A1 1 0 1 1 13.7 1.7L6.41 8.99z"
    />
  </svg>
);
export default SvgCalendarLeftArrow;
