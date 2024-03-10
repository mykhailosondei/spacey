import * as React from "react";
import type { SVGProps } from "react";
const SvgLuggage = (props: SVGProps<SVGSVGElement>) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    aria-hidden="true"
    style={{
      display: "block",
      height: 24,
      width: 24,
    }}
    viewBox="0 0 32 32"
    width="1em"
    height="1em"
    {...props}
  >
    <path d="M20 2a2 2 0 0 1 2 1.85V6h4a5 5 0 0 1 5 4.78V25a5 5 0 0 1-4.78 5H6a5 5 0 0 1-5-4.78V11a5 5 0 0 1 4.78-5H10V4a2 2 0 0 1 1.85-2H12zm-8 10.59 3 3V24H7v-8.41l3-3V8H6a3 3 0 0 0-3 2.82V25a3 3 0 0 0 2.82 3H26a3 3 0 0 0 3-2.82V11a3 3 0 0 0-2.82-3H12zm-1 1.83-2 2V22h4v-5.58zM20 4h-8v2h8z" />
  </svg>
);
export default SvgLuggage;
